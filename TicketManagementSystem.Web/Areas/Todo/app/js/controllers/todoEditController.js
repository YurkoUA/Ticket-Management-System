(function () {
    'use strict';

    angular
        .module('todoApp')
        .controller('todoEditController', ['$scope', '$rootScope', '$window', '$timeout', '$route', '$location', '$templateCache', '$routeParams', 'todoService', todoEditController]);

    function todoEditController($scope, $rootScope, $window, $timeout, $route, $location, $templateCache, $routeParams, todoService) {
        $scope.selectedTask;
        $scope.formSuccessMessage;
        $scope.formErrors;

        $scope.$on('$routeChangeStart', function (event, next, current) {
            if (typeof (current) != undefined) {
                $templateCache.remove(next.templateUrl);
                console.log("Route change start");
            }
        });

        $scope.$on('$routeChangeSuccess', function () {
            var id = $routeParams["id"];
            console.log("Route change success");

            $window.scrollTo(0, 0);

            if (id !== undefined) {
                todoService.getById(id).then(function (response) {
                    $scope.selectedTask = response.data;
                    $rootScope.title = "Редагувати задачу \"" + response.data.Title + "\"";
                    console.log("The task ID: " + id + "loaded.");
                }, function (response) {
                    console.log(response);
                });
            }
        });

        $scope.editTask = function (title, description, editForm) {
            if (!editForm.$valid)
                return;

            $scope.formSuccessMessage = undefined;
            $scope.formErrors = undefined;

            todoService.edit($scope.selectedTask.Id, title, description)
                .then(function (response) {
                    console.log("The task ID: " + $scope.selectedTask.Id + " succesfully edited.");
                    $scope.formSuccessMessage = 'Зміни збережено';

                    $timeout(function () {
                        $location.path("/details/" + $scope.selectedTask.Id);
                    }, 1000);
                }, function (response) {
                    $scope.formErrors = response.data;
                });
        };
    }
})();
