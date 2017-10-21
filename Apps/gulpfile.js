var gulp = require('gulp');
var clean = require('gulp-clean');
var gulpSequence = require('gulp-sequence')
const UglifyJSPlugin = require('uglifyjs-webpack-plugin')

var webpack = require('webpack');
var webpackStream = require('webpack-stream');
var webpackDevServer = require('webpack-dev-server');

//importing webpack configs
var common = require('./webpack.common');
//var modules = require('./webpack.modules');

var base = {
    dist: "dist/debug/",    
    debug: "dist/debug/",
    release:"dist/release/"
};

var config = {
    assetsSrc: "assets/**/*.*",   
    appsDest:base.dist+"apps",
    assetsDest: base.dist + "assets",
    htmlSrc:"views/**/*.html",
    htmlDest : base.dist,
    DistSrc: base.dist+"**/*.*",
    tsSrc: "src/**/*.ts",
    sourceMapSrc: "src/**/*.js.map",
    srcDest:base.dist + "apps",
    wwwroot:"../ETManage.Admin/wwwroot"
};

gulp.task('clean', function () {
    return gulp.src(base.dist, {read: false})
        .pipe(clean());
    });

gulp.task('clean-app', function () {
    return gulp.src(config.appsDest, {read: false})
        .pipe(clean());
    });
    
gulp.task('build:common', function() {
    return gulp.src('src/entry.js')
        .pipe(webpackStream(common))      
        .pipe(gulp.dest(config.appsDest+'/common'));
  });

gulp.task('build:modules', function() {
    /*return gulp.src('src/entry.js')
        .pipe(webpackStream(modules))      
        .pipe(gulp.dest(config.appsDest));*/
  });

gulp.task("copy:assets", function () {
    return gulp.src(config.assetsSrc)
        .pipe(gulp.dest(config.assetsDest));
});

gulp.task("copy:html", function () {
    return gulp.src(config.htmlSrc)
        .pipe(gulp.dest(config.htmlDest));
});
gulp.task("copy:js", function () {
    return gulp.src(config.jsSrc)
        .pipe(gulp.dest(config.jsDest));
});

gulp.task("copy:ts", function () {
    return gulp.src(config.tsSrc)
        .pipe(gulp.dest(config.srcDest));
});

gulp.task("copy:sourceMap", function () {
    return gulp.src(config.sourceMapSrc)
        .pipe(gulp.dest(config.srcDest));
});

gulp.task('webpack-dev-server', function(callback) {
    // modify some webpack config options

    var myConfig = Object.create(common);
    myConfig.devtool = 'source-map';
    myConfig.debug = true;

    // Start a webpack-dev-server
    new WebpackDevServer(webpack(myConfig), {
        stats: {
            colors: true
        },
        contentBase: 'dist/'
    }).listen(8080, 'localhost', function(err) {
        if(err) throw new gutil.PluginError('webpack-dev-server', err);
        gutil.log('[webpack-dev-server]', 'http://localhost:8080/index.html');
        proxy.run();
    });
});

gulp.task("copy:wwwroot", function () {
    return gulp.src(config.DistSrc)
    .pipe(gulp.dest(config.wwwroot));
});

gulp.task("setup-release",function(){
    base.dist = base.release;

    var prod=new webpack.DefinePlugin({
        'process.env': {
          NODE_ENV: '"production"'
        }
      });      
    common.plugins.push(prod);
    var uglify=new UglifyJSPlugin();
    common.plugins.push(uglify);
});

gulp.task("setup-debug",function(){
    base.dist = base.debug
});

gulp.task("build", 
    ["build:common",
    "build:modules",
    "copy:html"]);

gulp.task("build-release",gulpSequence("setup-debug", "clean-app",
    "build"
));

gulp.task("build-debug",gulpSequence("setup-debug", "clean-app","build",
    ["copy:ts",
    "copy:sourceMap"],
    "copy:wwwroot"
));

gulp.task("release", gulpSequence("setup-release", "clean",
    ["copy:assets",
    "build-release"],
    "copy:wwwroot"
));
        
gulp.task("debug", gulpSequence(
    "build-debug",
    "webpack-dev-server"
));

gulp.task('default',['build-debug']);