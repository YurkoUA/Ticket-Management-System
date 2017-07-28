(function () {
    'use strict';

    angular
        .module('todoApp')
        .factory('todoService', ['$http', todoService]);

    function todoService($http) {
        var service = {
            getAll: getAllTasks,
            getGrouped: getTasksGroupedByStatus,
            getByStatus: getTasksByStatus,
            getById: getTaskById,
            create: createTask,
            edit: editTask,
            delete: deleteTask,
            setStatus: setStatus,
            setPriority: setPriority,

            prioritiesList: getPriorities,
            statusList: getStatusList,

            getStatusByValue: getStatusByValue,
            getPriorityByValue: getPriorityByValue
        };

        var baseUrl = "/Todo/";

        return service;

        function getAllTasks() {
            return $http.get(baseUrl + "GetAllTasks")
        }

        function getTasksGroupedByStatus() {
            return $http.get(baseUrl + "GetGroupedTasks");
        }

        function getTasksByStatus(status) {
            return $http.get(baseUrl + "GetTasks", { params: { status: status } });
        }

        function getTaskById(id) {
            return $http.get(baseUrl + "Get", { params: { id: id } });
        }

        function createTask(title, description, priority) {
            return $http.post(baseUrl + "Create", { Title: title, Description: description, Priority: priority });
        }

        function editTask(id, title, description) {
            return $http.put(baseUrl + "Edit", { Title: title, Description: description }, { params: { id: id } });
        }

        function deleteTask(id) {
            return $http.delete(baseUrl + "Delete", { params: { id: id } });
        }

        function setStatus(taskId, status) {
            return $http.put(baseUrl + "SetStatus", status, { params: { id: taskId } });
        }

        function setPriority(taskId, priority) {
            return $http.put(baseUrl + "SetPriority", priority, { params: { id: taskId } });
        }

        function getPriorities() {
            return [{
                value: 1,
                text: "Дуже низький",
                textENG: "VeryLow"
            }, {
                value: 2,
                text: "Низький",
                textENG: "Low"
            }, {
                value: 3,
                text: "Середній",
                textENG: "Medium"
            }, {
                value: 4,
                text: "Високий",
                textENG: "High"
            }, {
                value: 5,
                text: "Дуже високий",
                textENG: "VeryHigh"
            }];
        }

        function getStatusList() {
            return [{
                value: 0,
                text: "Вільна",
                textENG: "None"
            }, {
                value: 1,
                text: "В процесі",
                textENG: "InProgress"
            }, {
                value: 2,
                text: "Перероблюється",
                textENG: "Recycle"
            }, {
                value: 3,
                text: "Виконана",
                textENG: "Completed"
            }];
        }

        function getStatusByValue(val) {
            var statusList = getStatusList();

            for (var i in statusList) {
                if (val === statusList[i].value) {
                    console.log("Returned " + statusList[i]);
                    return statusList[i];
                }
            }
            return undefined;
        }

        function getPriorityByValue(val) {
            var priorities = getPriorities();

            for (var i in priorities) {
                if (val === priorities[i].value) {
                    return priorities[i];
                }
            }
            return undefined;
        }
    }
})();