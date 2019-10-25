var app = angular.module("myApp", ["ng","ui.router", "ngStorage", "ui.bootstrap", "tm.pagination", "directives", "services"]);
app.controller('fatherCtrl', function ($scope, $http, $window, $location, ModalService) {
    $scope.$on("headSearch", function (event, data) {
        $scope.$broadcast("goHeadSearch", data);
    });
});
    



