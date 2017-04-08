const path = require('path');
const webpack = require('webpack');
const bundleOutputDir = './wwwroot/dist';

module.exports = {
    stats: { colors: true, reasons: true, modules: false },
    entry: { 'bundle': './ClientApp/boot.tsx' },
    output: {
        path: path.join(__dirname, bundleOutputDir),
        filename: '[name].js',
        publicPath: '/dist/'
    },
    resolve: { extensions: [ '.js', '.jsx', '.ts', '.tsx' ] },
    module: {
        rules: [
            { test: /\.ts(x?)$/, include: /ClientApp/, use: 'babel-loader' },
            { test: /\.tsx?$/, include: /ClientApp/, use: 'awesome-typescript-loader?silent=true' },
            { test: /\.css$/, use: [ 'style-loader', 'css-loader'] },
            { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' }
        ]
    }
}