module.exports = {
  devServer: {
    historyApiFallback: true,
    open: true,
    compress: true,
    hot: true,
    proxy: 'https://localhost:5001/'
  }
}