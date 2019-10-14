app.controller('MyCtrl', function ($scope, $http, $window, $location, ModalService) {
    $scope.user = {};

    $scope.reloadUser = function () {
        $http.get(GetUrl('api/Account/CurrentUser')).then(function (res) {
            $scope.user = res.data;
        }).catch(function (resp) {
            console.log(resp);
        });
    }
    $scope.reloadUser();

    $scope.GoSave = function () {
        if (!$scope.moneykey || $scope.moneykey.length < 10 || $scope.moneykey.length > 20) {
            ModalService.Message("请输入正确的卡密", "提示");
        } else {
            $http.post(GetUrl('api/MoneyKey/UsedMk?mk=' + $scope.moneykey)).then(function (res) {
                $scope.reloadUser();
                $scope.moneykey = "";
            }).catch(function (resp) {
                console.log(resp);
            });
        }
    }

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