app.controller('ProductCtrl', function ($scope, $http, $rootScope, $routeParams, $location, ModalService) {
    var pid = $routeParams.id;
    var invCode = $routeParams.invcode;
    $scope.product = $rootScope.SelectedProduct;

    //邀请人
    if (invCode) {

    }
    if (!pid || !$scope.product || isNaN(Number(pid))) {
        $location.path("/productList")
    }

    $scope.Pay = function () {
        ModalService.Confirm("确认购买?", function () {
            alert(1);
        });
    }
});
