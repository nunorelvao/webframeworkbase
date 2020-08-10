/// <binding BeforeBuild='default' Clean='1_CLEAN' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    sass = require("gulp-sass"),
    autoprefixer = require("autoprefixer"),
    sourcemaps = require("gulp-sourcemaps"),
    postcss = require("gulp-postcss"),
    rename = require("gulp-rename"),
    lodash = require("lodash"),
    minify = require('gulp-minifier');


var paths = {
    webroot: "./wwwroot/"
};

paths.js = "source/scripts/**/*.js";
paths.css = "source/scss/compiled_css" + "/**/*.css";
paths.cssmap = "source/scss/compiled_css/**/*.map";
paths.scss = "source/scss/**/*.scss";
paths.destCompiledCss = "source/scss/compiled_css/";
paths.jsDest = paths.webroot + "js/";
paths.concatJsDest = paths.webroot + "js/site.js";
paths.cssDest = paths.webroot + "css/";
paths.concatCssDest = paths.webroot + "css/site.css";
paths.sourceImages = "source/images/";
paths.imagesDest = paths.webroot + "images/";
paths.libDest = paths.webroot + "lib/";

//clean all destination files
//******** CLEAN  START ******************

gulp.task("clean:js", function (cb) {
    console.log("Cleaning JavaScript Destination Files");
    return rimraf(paths.jsDest + "*", cb);
});

gulp.task("clean:css", function (cb) {
    console.log("Cleaning Source CSS Files");
    //rimraf(paths.css, cb);
    return rimraf(paths.css + "*", cb);
});
gulp.task("clean:cssmap", function (cb) {
    console.log("Cleaning Source CSS Map Files");
    return rimraf(paths.cssmap, cb);
});
gulp.task("clean:cssDest", function (cb) {
    console.log("Cleaning CSS Compiled Destination Files");
    return rimraf(paths.cssDest + "*", cb);
});
gulp.task("clean:images", function (cb) {
    console.log("Cleaning Images Destination Files");
    //rimraf(paths.imagesDest + "*.*", cb);
    return rimraf(paths.imagesDest + "*", cb);
});
gulp.task("clean:lib", function (cb) {
    console.log("Cleaning lib Destination Files");
    //rimraf(paths.libDest + "*.*", cb);
    return rimraf(paths.libDest + "*", cb);
});



//******** CLEAN END ******************

//******** COMPILE START ******************
//compile sass & autoprefix
gulp.task("css-sass-autoprefix", function () {
    console.log("Compiling SASS and Autoprefixing from browserlist in packages.json");
    return gulp.src(paths.scss)
        .pipe(sass().on('error', sass.logError))
        .pipe(sourcemaps.init())
        .pipe(postcss([autoprefixer()]))
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest(paths.destCompiledCss));
});

//move images
gulp.task('move-images', function () {
    console.log("Moving all files in source/images folder");

    return gulp.src([paths.sourceImages + "**", '!' + paths.sourceImages + "svg", '!' + paths.sourceImages + "favicons"])
        .pipe(rename({ dirname: '' }))//remove dir from source to combine into one
        .pipe(gulp.dest(paths.imagesDest));
});

//******** COMPILE END ******************

//******** BUILD START ******************

gulp.task('copy-npm-js-css', function (cb) {
   
        console.log("Moving all NPM LIB files in wwwroot\js or wwwroot\css");
        var assets = {
            js: [
                './node_modules/bootstrap/dist/js/bootstrap.js',
                './node_modules/popper.js/dist/umd/popper.js',
                './node_modules/popper.js/dist/umd/popper-utils.js',
                './node_modules/jquery/dist/jquery.js',
                './node_modules/jquery-validation/dist/jquery.validate.js',
                './node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js',
                './node_modules/jquery-ajax-unobtrusive/jquery.unobtrusive-ajax.js',
                './node_modules/jquery-ui-bundle/jquery-ui.js',
                './node_modules/bootstrap-colorpicker/dist/js/bootstrap-colorpicker.js'
               

            ],
            css: [
                './node_modules/bootstrap/dist/css/bootstrap.css',
                './node_modules/jquery-ui-bundle/jquery-ui.css',
                './node_modules/jquery-ui-bundle/images*/*',
                './node_modules/bootstrap-colorpicker/dist/css/bootstrap-colorpicker.css'
            ]
        };
        lodash(assets).forEach(function (assets, type) {
            console.log(assets + " " + type);
            return gulp.src(assets)
                .pipe(gulp.dest(paths.webroot + type));
        });
       cb();
});

gulp.task('copy-npm-libs', function (cb) {
    console.log("Moving all NPM LIB files in wwwroot\lib folder");
    var assets = {
        "open-iconic": ['./node_modules/open-iconic/**/*']
    };

      lodash(assets).forEach(function (assets, type) {
         console.log(assets + " " + type);
        return gulp.src(assets)
            .pipe(gulp.dest(paths.webroot + "/lib/" + type));
    });
    cb();
});

//normal js
gulp.task("create:js", function (cb) {
    console.log("Moving and renaming compiled JavaScript");
    return gulp.src(paths.js, { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(gulp.dest("."));
});

//normal css
gulp.task("create:css", function (cb) {
    console.log("Moving and renaming compiled CSS");
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(gulp.dest("."));
});

//minify css
gulp.task('minifycss', function (cb) {

    setTimeout(function () {

        gulp.src(paths.cssDest + '*.*')
            .pipe(minify({
                minify: true,
                collapseWhitespace: true,
                conservativeCollapse: true,
                minifyJS: true,
                minifyCSS: true,
                getKeptComment: function (content, filePath) {
                    var m = content.match(/\/\*![\s\S]*?\*\//img);
                    return m && m.join('\n') + '\n' || '';
                }
            }))
            .pipe(rename({
                suffix: '.min'
            }))
            .pipe(gulp.dest(paths.cssDest));

    }, 500, null);
    cb();
});

//gulp.task('createandminifycss', ['create:css', 'minifycss']);

//minify css
gulp.task('minifyjs', function (cb) {

    setTimeout(function () {

        gulp.src(paths.jsDest + '*.*')
            .pipe(minify({
                minify: true,
                collapseWhitespace: true,
                conservativeCollapse: true,
                minifyJS: true,
                minifyCSS: true,
                getKeptComment: function (content, filePath) {
                    var m = content.match(/\/\*![\s\S]*?\*\//img);
                    return m && m.join('\n') + '\n' || '';
                }
            }))
            .pipe(rename({
                suffix: '.min'
            }))
            .pipe(gulp.dest(paths.jsDest));

    }, 500, null);
    cb();
});

//******** BUILD END ******************

gulp.task("1_CLEAN", gulp.series(gulp.parallel("clean:js", "clean:css", "clean:cssmap", "clean:cssDest", "clean:images", "clean:lib")));

gulp.task("2_COMPILE", gulp.series(gulp.parallel("css-sass-autoprefix", "move-images")));

gulp.task("3_BUILD", gulp.series(gulp.parallel("copy-npm-js-css", "copy-npm-libs", "create:css", "create:js")));

gulp.task("4_MINIFYALL",  gulp.series(gulp.parallel("minifycss", "minifyjs")));

gulp.task('default',  gulp.series("1_CLEAN", "2_COMPILE", "3_BUILD", "4_MINIFYALL"));
      

