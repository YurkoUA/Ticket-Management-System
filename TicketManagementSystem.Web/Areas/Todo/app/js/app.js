(function () {
    'use strict';

    angular.module('todoApp', ['ngRoute'])
        .config(function ($routeProvider) {
            $routeProvider.when("/", {
                controller: "todoController",
                templateUrl: "/Areas/Todo/app/views/taskList.html"    
            });

            $routeProvider.when("/create", {
                controller: "todoController",
                templateUrl: "/Areas/Todo/app/views/create.html"
            });

            $routeProvider.when("/details/:id", {
                controller: "todoController",
                templateUrl: "/Areas/Todo/app/views/details.html"
            });

            $routeProvider.when("/edit/:id", {
                controller: "todoEditController",
                templateUrl: "/Areas/Todo/app/views/edit.html"
            });

            $routeProvider.otherwise({ redirectTo: '/' });
        })
        .run(function ($rootScope) {
            $rootScope.title = "Головна";
        });
})();