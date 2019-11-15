var app = angular.module("myApp", ["ng", "ui.router", "ngStorage", "ui.bootstrap", "tm.pagination", "directives", "services"]);
app.controller('fatherCtrl', function ($scope, $http, $window, $location, $state, ModalService) {
    $scope.$on("headSearch", function (event, data) {
        $state.go("index.productList", { pname: data });
    });
});




