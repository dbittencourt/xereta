const path = require('path');
const webpack = require('webpack');
const bundleOutputDir = './wwwroot/dist';
const CheckerPlugin = require('awesome-typescript-loader');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
    stats: { modules: false },
    entry: { 'main': './ClientApp/boot.tsx' },
    resolve: { extensions: [ '.js', '.jsx', '.ts', '.tsx' ] },
    output: {
        path: path.join(__dirname, bundleOutputDir),
        filename: '[name].js',
        publicPath: '/dist/'
    },
    module: {
        rules: [
            { test: /\.ts(x?)$/, include: /ClientApp/, use: 'babel-loader' },
            { test: /\.tsx?$/, include: /ClientApp/, use: 'awesome-typescript-loader?silent=true' },
            //{ test: /\.css$/, use: ExtractTextPlugin.extract('style-loader!css-loader') },
            { test: /\.css$/, use: ['style-loader', 'css-loader'] },
            { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' }
        ]
    },
    plugins: [
        //new CheckerPlugin(),
        new ExtractTextPlugin('site.css')
    ]
};
