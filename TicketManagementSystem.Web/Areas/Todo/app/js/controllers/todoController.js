(function () {
    'use strict';

    angular
        .module('todoApp')
        .controller('todoController', ['$scope', '$rootScope', '$window', '$route', '$location', '$templateCache', '$routeParams', 'todoService', todoController]);

    function todoController($scope, $rootScope, $window, $route, $location, $templateCache, $routeParams, todoService) {
        console.log("todoController is running...");

        $rootScope.title = "Головна";

        todoService.getGrouped()
            .then(function (response) {
                if (response.status !== 204) {
                    console.log("Tasks loaded");
                    $scope.taskList = response.data;
                } else {
                    $scope.taskList = undefined;
                    console.log("No content");
                }
            }, function (error) {
                console.log(response);
            });

        $scope.priorities = todoService.prioritiesList();

        $scope.$on('$routeChangeStart', function (event, next, current) {
            if (typeof (current) != undefined) {
                $templateCache.remove(next.templateUrl);
                console.log("Route change start");
            }
        });

        $scope.$on('$routeChangeSuccess', function () {
            var id = $routeParams["id"];
            console.log("Route change success. ID: " + id);

            $window.scrollTo(0, 0);

            if (id !== undefined) {
                console.log(id);

                todoService.getById(id).then(function (response) {
                    $scope.selectedTask = response.data;
                    $rootScope.title = "Задача \"" + response.data.Title + "\"";

                    $scope.priorities = todoService.prioritiesList();
                    console.log("Task ID: " + id + "loaded. Status: " + $scope.selectedTask.Status);
                }, function (response) {
                    console.log(response);
                });
            }
        });


        $scope.createTask = function (title, description, priority, createForm) {
            if (!createForm.$valid)
                return;

            $scope.formSuccessMessage = undefined;
            $scope.formErrors = undefined;

            todoService.create(title, description, priority)
                .then(function (response) {
                    $scope.formSuccessMessage = 'Задачу успішно додано.';
                    console.log("Tasks created as ID: " + response.data.Id);

                    $scope.Title = undefined;
                    $scope.Description = undefined;
                    $scope.Priority = undefined;

                    createForm.$setUntouched();
                    document.getElementById("createForm").reset();
                }, function (response) {
                    $scope.formErrors = response.data;
                });
        };

        $scope.deleteTask = function (task, status) {
            if (confirm("Видалити задачу \"" + task.Title + "\"?") === true) {
                todoService.delete(task.Id)
                    .then(function (response) {
                        var index = $scope.taskList[status].indexOf(task);

                        $scope.taskList[status].splice(index, 1);
                        
                        console.log("The task ID: " + task.Id + " succesfully deleted.");

                        $scope.$apply();
                        console.log("Scope is applied");

                        $templateCache.removeAll();
                        console.log("Cache is removed");

                        return true;
                    }, function (response) {
                        console.log(response);
                    });
            }
        };

        $scope.deleteSelectedTask = function (id) {
            if (confirm("Дійсно видалити задачу \"" + $scope.selectedTask.Title + "\"?") === true) {
                todoService.delete(id)
                    .then(function (response) {
                        $location.path("/");
                    }, function (response) {

                    });
            }
        };

        $scope.setStatus = function (id, status) {
            todoService.setStatus(id, status)
                .then(function (response) {
                    $scope.selectedTask.Status = status;
                    $scope.selectedTask.StatusString = todoService.getStatusByValue(status).text;
                    
                    console.log("Status: " + $scope.selectedTask.Status + "\n" + $scope.selectedTask.StatusString);
                    
                    $templateCache.removeAll();
                }, function (response) {
                    console.log(response);
                });
        };

        $scope.setPriority = function (id, priority) {
            todoService.setPriority(id, priority)
                .then(function (response) {
                    $scope.selectedTask.Priority = priority;
                    $scope.selectedTask.PriorityString = todoService.getPriorityByValue(priority).text;

                    $scope.priorities = todoService.prioritiesList();
                    $templateCache.removeAll()
                }, function (response) {
                    console.log(response);
                });
        };
    }
})();
