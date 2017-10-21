var webpack = require("webpack");
var path = require('path');

module.exports = {
    entry: {
        "menu/menu":"./src/common/menu/menu.ts",
        common:["jquery","vue"]
    },
    output: {
        filename: "[name].js"
    },
    resolve: {
        extensions: [".ts","vue", ".js"],
        alias: {
            'vue': 'vue/dist/vue.js'
        }
    },
    module: {
        loaders: [
            { test: /\.vue$/, loader: 'vue' },
            { test: /\.ts$/, loader: 'ts-loader' },
            { test: /\.html$/, loader:'html-loader',
                options:{
                    transpileOnly:true
                }
            }
        ]
    },
    plugins: [
      new webpack.optimize.CommonsChunkPlugin({name:"common",  filename :"common.bundle.js"})
    ]
};