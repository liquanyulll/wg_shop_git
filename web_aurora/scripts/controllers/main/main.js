﻿app.controller('headCtrl', function ($scope, $http, $window, $state) {
    $scope.user = {};

    //加载商品类型
    $http.get(GetUrl('api/Account/CurrentUser')).then(function (res) {
        $scope.user = res.data;
    }).catch(function (resp) {
        console.log(resp);
    });

    $scope.SignOut = function () {
        $window.localStorage.removeItem("ApiToken");
        window.location = "login.html";
    };

    $scope.GoUserCenter = function () {
        $state.go("index.my");
    };

    $scope.headSearch = function () {
        $scope.$emit("headSearch", $scope.headSearchName);
    }
});

app.controller('navbarCtrl', function ($scope, $http, $window, $location) {
    $scope.ProductTypes = [];

    //加载商品类型
    $http.get(GetUrl('api/ProductType/ProductTypes')).then(function (res) {
        $scope.ProductTypes = res.data;
        console.log($scope.ProductTypes);
    }).catch(function (resp) {
        console.log(resp);
    });
});
