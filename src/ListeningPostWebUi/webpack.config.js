'use strict'
const Path = require('path')
const webpack = require('webpack')
const MiniCssExtractPlugin = require('mini-css-extract-plugin')
const { VueLoaderPlugin } = require('vue-loader')
const htmlWebpackPlugin = require('html-webpack-plugin')
const { CleanWebpackPlugin } = require('clean-webpack-plugin')

const resolve = (path) => Path.resolve(__dirname, path)

module.exports = env => {
  const devMode = env.development
  return {
    mode: devMode ? 'development' : 'production',
    entry: {
      app: './src/main.js'
    },
    devtool: devMode ? 'eval-source-map' : false,
    output: {
      path: resolve('dist'),
      filename: devMode ? '[name].bundle.js' : 'js/[name].[contenthash].bundle.js',
      chunkFilename: devMode ? '[name].bundle.js' : 'js/[name].[chunkhash].bundle.js',
      publicPath: '/'
    },
    resolve: {
      extensions: ['*', '.js', '.vue', '.json'],
      alias: {
        'vue$': 'vue/dist/vue.runtime.esm.js',
        '@': resolve('src')
      }
    },
    module: {
      rules: [
        {
          test: /\.(sa|sc|c)ss$/,
          use: [
            devMode ? 'style-loader' : MiniCssExtractPlugin.loader,
            {
              loader: 'css-loader', options: {
                sourceMap: true,
                importLoaders: devMode ? 1 : 2,
                // modules: true
              }
            },
            { loader: 'postcss-loader', options: { sourceMap: true } },
            { loader: 'sass-loader', options: { sourceMap: true } }
          ]
        },
        {
          test: /\.vue$/,
          loader: 'vue-loader'
        },
        {
          test: /\.js$/,
          exclude: /node_modules/,
          loader: 'babel-loader',
          include: [resolve('src'), resolve('test')]
        },
        {
          test: /\.(?:icon?|png|jpe?g|gif|svg|mp4|webm|ogg|mp3|wav|flac|aac|woff2?|eot|(o|t)tf)(\?.*)?$/,
          type: 'asset'
        }
      ]
    },
    plugins: [
      ...(devMode // Exclusive plugins
        ? [new webpack.HotModuleReplacementPlugin()]
        : [new MiniCssExtractPlugin()]),
      ...[ // Common plugins
        new VueLoaderPlugin(),
        new CleanWebpackPlugin(),
        new htmlWebpackPlugin({
          title: 'Things',
          favicon: resolve('src/assets/logo.png'),
          template: resolve('src/index.html'),
          xhtml: true
        })
      ]
    ],
    optimization: devMode ? undefined : {
      splitChunks: {
        chunks: 'all',
        cacheGroups: {
          vendors: {
            test: /[\\/]node_modules[\\/]/,
            priority: -10
          }
        }
      },
      runtimeChunk: {
        name: 'runtime'
      }
    },
    devServer: {
      historyApiFallback: true,
      open: true,
      compress: true,
      hot: true,
      proxy: {
        '/api': 'https://localhost:5001/'
      }
    }
  }
}
