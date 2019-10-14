app.controller('PreProcessCtrl', ['$scope', '$http', "ModalService", function ($scope, $http, ModalService) {
    $scope.cbWebSites = [];
    $scope.preProcesss = [];

    $scope.queryParams = {
        WebSiteId: null
    };
    $scope.paginationConf = {
        currentPage: 1,
        itemsPerPage: 15
    };
    $scope.paginationConf.perPageOptions = [15, 20, 50, 100];

    //加载站点下拉框
    $http.get(GetUrl('api/WebSite/GetCombobox')).then(function (res) {
        $scope.cbWebSites = res.data;
    }).catch(function (resp) {
        console.log(resp);
        });
    //加载国家下拉框
    $http.get(GetUrl('api/MBCommon/GetCountrys')).then(function (res) {
        $scope.countrys = res.data;
    }).catch(function (resp) {
        console.log(resp);
    })

    $scope.GetStateName = function (val) {
        switch (val) {
            case 1:
                return "待审核";
            case 2:
                return "待发布";
            case 3:
                return "已发布";
            default:
                return "未知";
        }
    };

    //查询列表数据
    $scope.search = function () {
        $scope.queryParams.pageIndex = $scope.paginationConf.currentPage;
        $scope.queryParams.pageSize = $scope.paginationConf.itemsPerPage;

        $http.post(GetUrl('api/PreProcess/Search'), $scope.queryParams).then(function (res) {
            $scope.preProcesss = res.data.data;
            $scope.paginationConf.totalItems = res.data.data.total;
        }).catch(function (resp) {
            console.log(resp);
        });
    };

    /* 监听数据类型切换及换页 */
    $scope.$watch('paginationConf.currentPage + paginationConf.itemsPerPage', function (v, a) {
        $scope.search();
    });

    //监听数据是否改变
    $scope.$watch('preProcess.Process', function (newValue, oldValue) {
        if ($scope.selectedX) {
            $scope.unChanged = angular.equals($scope.selectedX.Process, newValue);
        }
    }, true);

    //审核通过
    $scope.Audit = function (x) {
        ModalService.Confirm("确认审核通过?", function () {
            $http.post(GetUrl('api/PreProcess/Audit?id='+x.id)).then(function (res) {
                $scope.search();
            }).catch(function (resp) {
                console.log(resp);
            });
        });
    };

    /// 弹层相关
    $scope.preProcess = {
        Process: {}
    };

    //弹出详情框框
    $scope.Edit = function (x) {
        $scope.selectedX = x;
        $scope.selectedX.Process = JSON.parse(x.processJson);
        $scope.preProcess = angular.copy($scope.selectedX);
        $('#detailModal').modal({
            keyboard: true,
            backdrop: true
        });
    };
    //保存修改后的数据
    $scope.Save = function () {
        $scope.isSubmit = true;
        if (!$scope.preForm.$valid)
            return;

        $scope.preProcess.processJson = JSON.stringify($scope.preProcess.Process);
        $http.post(GetUrl('api/PreProcess/Update'), $scope.preProcess).then(function (res) {
            $('#detailModal').modal('hide');
            $scope.search();
        }).catch(function (resp) {
            console.log(resp);
        });
    };
}]);