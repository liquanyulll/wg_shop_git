app.controller('MyCtrl', function ($scope, $http, $window, $location) {
    $scope.user = {};

    $http.get(GetUrl('api/Account/CurrentUser')).then(function (res) {
        $scope.user = res.data;
    }).catch(function (resp) {
        console.log(resp);
    });

    //$scope.SignOut = function () {
    //    $window.localStorage.removeItem("ApiToken");
    //    window.location = "login.html";
    //};

    //$scope.ProductTypes = [];


    ////加载商品类型
    //$http.get(GetUrl('api/ProductType/ProductTypes')).then(function (res) {
    //    $scope.ProductTypes = res.data;
    //}).catch(function (resp) {
    //    console.log(resp);
    //});
});