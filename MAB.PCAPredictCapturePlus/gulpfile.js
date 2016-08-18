/// <binding />
"use strict";

var gulp = require('gulp'),
    path = require('path'),
    exec = require('child_process').exec,
    fs = require('fs');

var solutionFolder = path.resolve(__dirname, '..');
var projectFolder = path.join(solutionFolder, 'MAB.PCAPredictCapturePlus');
var distFolder = path.join(solutionFolder, 'dist');

if (!fs.existsSync(distFolder)) {
    fs.mkdirSync(distFolder);
}

gulp.task('nuget-clean', function (callback) {
    exec('del *.nupkg', { cwd: distFolder }, function (err, stdout, stderr) {
        console.log(stdout);
        console.log(stderr);
        callback(err);
    });
});

gulp.task('nuget-pack', ['nuget-clean'], function (callback) {
    exec('nuget pack MAB.PCAPredictCapturePlus.csproj -OutputDirectory ' + distFolder + ' -Prop Configuration=Release', { cwd: projectFolder }, function (err, stdout, stderr) {
        console.log(stdout);
        console.log(stderr);
        callback(err);
    });
});

gulp.task('nuget-push', ['nuget-pack', 'nuget-clean'], function(callback) {
    exec('publish.cmd', { cwd: solutionFolder }, function (err, stdout, stderr) {
        console.log(stdout);
        console.log(stderr);
        callback(err);
    });
});
