const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin');
const TerserPlugin = require('terser-webpack-plugin');
const merge = require('webpack-merge');
const Dotenv = require('dotenv-webpack');

// Common configurations for both development and production environment.
const commonConfig = {
  mode: "production",
  resolve: {
    extensions: ['.js', '.ts', '.tsx'],
    modules: [path.resolve('./ClientApp'), path.resolve('./node_modules'), path.resolve('../shared')],
  },
  entry: {
    main: './ClientApp/boot-client.tsx'
  },
  output: {
    filename: '[name].js',
    path: path.resolve(__dirname, 'wwwroot', 'dist'),
    publicPath: '/dist/'
  },
  module: {
    rules: [
      {
        test: /\.(jpeg|jpg|woff|woff2|eot|ttf|svg)(\?.*$|$)/,
        use: [
          {
            loader: 'url-loader',
            options: {
              limit: 100000
            }
          }
        ]
      }
    ]
  },
  plugins: [
    new webpack.DllReferencePlugin({
      context: __dirname,
      manifest: require('./wwwroot/dist/library.json')
    }),
    new webpack.DllReferencePlugin({
      context: __dirname,
      manifest: require('./wwwroot/dist/vendor.json')
    }),
    new Dotenv({
      systemvars: true,
    })
  ]
};

// Configuration for development environment. Will be merged with commonConfig.
const devConfig = {
  mode: "development",
  devtool: 'inline-source-map',
  module: {
    rules: [
      {
        test: /\.(css|scss)$/i,
        use: ['style-loader', "css-loader", "sass-loader"]
      },
      {
        test: /\.tsx?$/,
        exclude: path.resolve('./node_modules'),
        use: [
          'babel-loader',
          {
            loader: 'ts-loader',
            options: {
              transpileOnly: true
            }
          }
        ]
      }
    ]
  },
  plugins: [
    new ForkTsCheckerWebpackPlugin(),
  ]
};

// Configuration for production environment. Will be merged with commonConfig.
const prodConfig = {
  mode: "production",
  module: {
    rules: [
      {
        test: /\.(css|scss)$/i,
        use: [MiniCssExtractPlugin.loader, "css-loader", "sass-loader"]
      },
      {
        test: /\.tsx?$/,
        exclude: path.resolve('./node_modules'),
        use: [
          'babel-loader',
          {
            loader: 'ts-loader',
            options: {
              transpileOnly: true
            }
          }
        ]
      }
    ]
  },
  optimization: {
    minimize: true,
    minimizer: [new TerserPlugin()],
  },
  plugins: [
    new ForkTsCheckerWebpackPlugin({
      async: false,
      useTypescriptIncrementalApi: true,
      memoryLimit: 4096
    }),
    new MiniCssExtractPlugin({
      filename: 'site.css',
    }),
  ]
};

// Conditional merge of commonConfig and devConfig/prodConfig.
module.exports = (env) => {
  if (env && env.prod) {
    return merge(commonConfig, prodConfig);
  } else {
    return merge(commonConfig, devConfig);
  }
};
