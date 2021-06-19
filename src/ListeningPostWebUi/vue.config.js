module.exports = {
  pages: {
    index: {
      entry: './src/main.js',
      template: './src/index.pug',
      // chunks: [
      //   'chunk-vendors',
      //   'chunk-common',
      //   'index'
      // ]
    }
  },
  // entry: {
  //   index: {
  //     template: './src/index.pug'
  //   }
  // },
  // chainWebpack: config => {
  //   config.module
  //   .rule('pug')
  //   .test(/\.pug$/)
  //   .use('pug-plain-loader')
  //   .loader('pug-plain-loader')
  //   .end()
  // },
  devServer: {
    historyApiFallback: true,
    open: true,
    compress: true,
    hot: true,
    proxy: 'https://localhost:5001/'
  }
}
