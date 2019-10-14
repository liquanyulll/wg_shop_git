app.controller('LoginCtrl', function ($scope, $http, $window, $location, $rootScope) {
    $scope.isSubmit = false;
    $rootScope.CurrentPage = "Login";
    //if (!$window.localStorage) {
    //    alert("浏览器支持localstorage");
    //}

    angular.element(".modal-backdrop").remove();
    angular.element("body").removeClass("modal-open");

    $scope.Save = function () {
        $scope.isSubmit = true;
        if (!$scope.loginForm.$valid)
            return;

        $http.get(GetUrl('api/UserAccount/Login?userName=' + $scope.UserName + "&password=" + $scope.Password)).then(function (res) {
            $window.localStorage.ApiToken = res.data;
            $rootScope.CurrentPage = "";
            $location.path("/dashboard");
        }).catch(function (resp) {
            console.log(resp);
        });
    };
});