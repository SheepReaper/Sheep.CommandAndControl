module.exports = {
  pages: {
    index: {
      entry: './src/main.js',
      template: './src/index.pug'
    }
  },
  devServer: {
    historyApiFallback: true,
    open: true,
    compress: true,
    hot: true,
    proxy: 'https://localhost:5001/'
  }
}
