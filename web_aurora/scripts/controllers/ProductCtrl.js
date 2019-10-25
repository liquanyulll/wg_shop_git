app.controller('ProductCtrl', function ($scope, $http, $rootScope, $stateParams, $state, ModalService) {
    var pid = $stateParams.id;
    var invCode = $stateParams.invcode;

    console.log($stateParams);

    //邀请人
    if (invCode) {

    }
    if (!pid || isNaN(Number(pid))) {
        $state.go("index.productList");
    }

    $scope.getProductInfo = function () {
        $http.post(GetUrl('api/Product/GetById?pid=' + pid), null).then(function (resp) {
            $scope.product = resp.data;
            $scope.invUrl = location.href;
            $scope.invUrl.
        }).catch(function (resp) {
            console.log(resp);
        });
    }

    $scope.getProductInfo();

    $scope.createInv = function () {
        $http.post(GetUrl('api/Account/CreateInvProduct?pid=' + pid), null).then(function (resp) {
            $scope.product.invInfo = resp.data;
        }).catch(function (resp) {
            console.log(resp);
        });
    }

    $scope.Pay = function (way) {
        ModalService.Confirm("确认购买?", function () {
            $http.post(GetUrl('api/Product/BuyProduct?way=' + way + '&pid=' + pid), null).then(function (resp) {
                $scope.getProductInfo();
            }).catch(function (resp) {
                console.log(resp);
            });
        });
    }
});
