"use strict";

const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const TerserPlugin = require('terser-webpack-plugin');
const merge = require('webpack-merge');

let excludedDependencies = ['font-awesome'];
let cssDependencies = ['bootstrap/dist/css/bootstrap.css', 'font-awesome/css/font-awesome.css']
let packageJson = require('./package.json');
let allDependencies = Object.keys(packageJson['dependencies']).filter(pkg => !excludedDependencies.includes(pkg));
let reactDependencies = allDependencies.filter(pkg => pkg.indexOf("react") > -1 || pkg.indexOf("redux") > -1);
let otherDependencies = allDependencies.filter(pkg => pkg.indexOf("react") === -1 && pkg.indexOf("redux") === -1);

// Common configurations for both development and production environment.
const commonConfig = {
  mode: 'production',
  resolve: {
    extensions: ['.js'],
    modules: [path.resolve('./node_modules')],
  },

  entry: {
    library: reactDependencies,
    vendor: [...otherDependencies, ...cssDependencies]
  },

  output: {
    filename: '[name].js',
    path: path.resolve(__dirname, 'wwwroot', 'dist'),
    publicPath: '/dist/',
    library: '[name]_[hash]',
  },

  module: {
    rules: [
      {
        test: /\.(png|woff|woff2|eot|ttf|svg)(\?|$)/,
        use: [
          {
            loader: 'url-loader',
            options: {
              limit: 10000,
            }
          }
        ]
      },
      {
        test: /\.(css|scss)$/i,
        use: [MiniCssExtractPlugin.loader, 'css-loader', 'sass-loader']
      }
    ]
  },

  plugins: [
    new webpack.DllPlugin({
      context: __dirname,
      entryOnly: true,
      path: path.join(__dirname, 'wwwroot', 'dist', '[name].json'),
      name: '[name]_[hash]'
    }),
    new MiniCssExtractPlugin({
      filename: 'vendor.css',
    }),
  ]
};

// Configuration for development environment. Will be merged with commonConfig.
const devConfig = {
  mode: 'development',
  devtool: 'cheap-module-source-map'
};

// Configuration for production environment. Will be merged with commonConfig.
const prodConfig = {
  mode: 'production',
  optimization: {
    minimize: true,
    minimizer: [new TerserPlugin()],
  },
};

// Conditional merge of commonConfig and devConfig/prodConfig.
module.exports = (env) => {
  if (env && env.prod) {
    return merge(commonConfig, prodConfig);
  } else {
    return merge(commonConfig, devConfig);
  }
};
