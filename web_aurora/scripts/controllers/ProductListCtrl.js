app.controller('ProductListCtrl', function ($scope, $http, $rootScope, $stateParams, $state) {

    console.log($stateParams);

    var init = function () {
        $scope.InfoList = [];
        $scope.queryVar = {};
        /* 配置分页基本参数 */
        $scope.paginationConf = {
            currentPage: 1,
            itemsPerPage: 20
        };
        $scope.paginationConf.perPageOptions = [20];
    }
    init();

    $scope.Search = function () {
        $scope.icc1++;
        $scope.queryVar.PageIndex = $scope.paginationConf.currentPage;
        $scope.queryVar.PageSize = $scope.paginationConf.itemsPerPage;
        $scope.queryVar.TypeId = $stateParams.tpid;

        $http.post(GetUrl('api/Product/Search'), $scope.queryVar).then(function (resp) {
            var qRtn = resp.data;
            $scope.InfoList = qRtn.contentList
            if ($scope.InfoList != null && $scope.InfoList.length > 0) {
                $scope.paginationConf.totalItems = qRtn.totalItems;
            } else {
                $scope.paginationConf.totalItems = 1;
            }
        }).catch(function (resp) {
            console.log(resp);
        });
    };
    $scope.Search();

    $scope.$on("goHeadSearch", function (event, data) {
        $scope.queryVar.PName = data;
        $scope.Search();
    });

    $scope.GoProduct = function (x) {
        $rootScope.SelectedProduct = x;
        $state.go("index.productList.product", { id: x.productId });
    }

    //$http.get(GetUrl('api/Account/CurrentUser')).then(function (res) {
    //    $scope.user = res.data;
    //}).catch(function (resp) {
    //    console.log(resp);
    //});

});
