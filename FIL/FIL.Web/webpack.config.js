const path = require('path');
const webpack = require('webpack');
const merge = require('webpack-merge');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin');
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const CssNanoPlugin = require("cssnano");
const TerserWebpackPlugin = require("terser-webpack-plugin");
const WatchMissingNodeModulesPlugin = require('react-dev-utils/WatchMissingNodeModulesPlugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const CopyPlugin = require('copy-webpack-plugin');
const Dotenv = require('dotenv-webpack');

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);
    var mode = isDevBuild ? "development" : "production";
    // Configuration in common to both client-side and server-side bundles
    const sharedConfig = () => ({
        stats: 'errors-only',
        mode,
        resolve: {
            modules: [path.resolve('./ClientApp'), path.resolve('./node_modules'), path.resolve('../shared')],
            extensions: ['.js', '.jsx', '.ts', '.tsx'],
            alias: {
                shared: "../shared",
                images: path.resolve(__dirname, './ClientApp/images')
            }
        },
        output: {
            filename: '[name].js',
            publicPath: '/dist/' // Webpack dev middleware, if enabled, handles requests for this URL prefix
        },
        module: {
            rules: [{ test: /\.css$/, use: [MiniCssExtractPlugin.loader, 'css-loader', 'sass-loader'] },
            { test: /\.(scss|sass)$/, use: [MiniCssExtractPlugin.loader, 'css-loader', 'sass-loader'] },
            {
                test: /\.tsx?$/, include: [
                    path.resolve(__dirname, './ClientApp'),
                    path.resolve(__dirname, '../shared')
                ], use: [{
                    loader: 'babel-loader',
                    options: {
                        compact: true
                    }
                },
                {
                    loader: 'ts-loader',
                    options: {
                        // Disable type checker - we will use it in fork plugin.
                        transpileOnly: true
                    }
                },
                    'ts-nameof-loader']
            }
            ]
        },
        plugins: [
            new ForkTsCheckerWebpackPlugin(), 
            new MiniCssExtractPlugin({ filename: 'site.css', allChunks: true }),
            new CopyPlugin([{ from: './ClientApp/css/zoom.css', to: 'zoom.css' }]),
            new Dotenv({
                systemvars: true,
            })
        ]
            .concat(isDevBuild ? [
                // Development.
                new WatchMissingNodeModulesPlugin(path.resolve(__dirname, '..', 'node_modules'))
            ] : [
                    new CleanWebpackPlugin({
                        cleanOnceBeforeBuildPatterns: [`*.hot-update.*`]
                    }), // Production.
                ]),
        performance: {
            hints: false,
        },
        optimization: {
            minimize: !isDevBuild,
            usedExports: isDevBuild,
            minimizer: !isDevBuild ? [
                // Production.
                new TerserWebpackPlugin({
                    terserOptions: {
                        output: {
                            comments: false,
                        },
                    },
                }),
                new OptimizeCSSAssetsPlugin({
                    cssProcessor: CssNanoPlugin,
                    cssProcessorPluginOptions: {
                        preset: ["default", { discardComments: { removeAll: true } }]
                    }
                })
            ] : []
        },
    });
    // Configuration for client-side bundle suitable for running in browsers
    const clientBundleOutputDir = './wwwroot/dist';
    var clientBundleConfig = merge(sharedConfig(), {
        entry: { 'main-client': './ClientApp/boot-client.tsx' },
        module: {
            rules: [
                { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=10000' }
            ]
        },

        output: { path: path.join(__dirname, clientBundleOutputDir) },
        plugins: [
            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require('./wwwroot/dist/vendor-manifest.json')
            })
        ].concat(isDevBuild ? [
            // Plugins that apply in development builds only
            new webpack.SourceMapDevToolPlugin({
                filename: '[file].map', // Remove this line if you prefer inline source maps
                moduleFilenameTemplate: path.relative(clientBundleOutputDir, '[resourcePath]') // Point sourcemap entries to the original file locations on disk
            })
        ] : [])
    });
    if (isDevBuild) {
        clientBundleConfig = {
            ...clientBundleConfig,
            performance: {
                hints: false,
            },
            devtool: 'eval-source-map'
        }
    }
    // Configuration for server-side (prerendering) bundle suitable for running in Node
    const serverBundleConfig = merge(sharedConfig(), {
        resolve: {
            mainFields: ['main'],
            // Look for modules in .ts(x) files first, then .js(x)
            extensions: ['*', '.ts', '.tsx', '.js', '.jsx']
        },
        entry: { 'main-server': './ClientApp/boot-server.tsx' },
        plugins: [
            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require('./ClientApp/dist/vendor-manifest.json'),
                sourceType: 'commonjs2',
                name: './vendor'
            }),
            //new WebpackNotifierPlugin({ alwaysNotify: true })
        ],
        module: {
            rules: [
                { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=10000' }
            ]
        },
        output: {
            libraryTarget: 'commonjs2',
            path: path.join(__dirname, './ClientApp/dist')
        },
        target: 'node'
    });

    return [clientBundleConfig, serverBundleConfig];
};