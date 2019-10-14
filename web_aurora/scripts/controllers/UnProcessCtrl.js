app.controller('UnProcessCtrl', function ($scope, $http) {
    $scope.cbWebSites = [];
    $scope.unProcesss = [];
    $scope.detailUnProcess = {};

    var nowDateTime = new Date();
    $scope.queryParams = {
        WebSiteId: null,
        Successful: "true",
        Date: nowDateTime.getFullYear() + '-' + appendZero(nowDateTime.getMonth() + 1) + "-" + appendZero(nowDateTime.getDate())
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
    });

    //查询列表数据
    $scope.search = function () {
        $scope.queryParams.pageIndex = $scope.paginationConf.currentPage;
        $scope.queryParams.pageSize = $scope.paginationConf.itemsPerPage;

        $http.post(GetUrl('api/UnProcess/Search'), $scope.queryParams).then(function (res) {
            
            $scope.unProcesss = res.data.data;
            $scope.paginationConf.totalItems = res.data.data.total;
        }).catch(function (resp) {
            console.log(resp);
        });
    };

    /* 监听数据类型切换及换页 */
    $scope.$watch('paginationConf.currentPage + paginationConf.itemsPerPage', function (v, a) {
        $scope.search();
    });


    /// 弹层相关
    $scope.Process = function (x) {
        $scope.preProcess = {
            Process: {}
        };

        $http.get(GetUrl('api/UnProcess/Get?id=' + x.id)).then(function (res) {
            $scope.detailUnProcess = res.data;
            $scope.detailUnProcess.resultJson = JSON.parse($scope.detailUnProcess.resultJson);

            if ($scope.detailUnProcess.preProcess && $scope.detailUnProcess.preProcess.length > 0) {
                $scope.preProcess = $scope.detailUnProcess.preProcess[0];
                $scope.preProcess.Process = JSON.parse($scope.preProcess.processJson);
            }

            $('#detailModal').modal({
                keyboard: true,
                backdrop: true
            });
        }).catch(function (resp) {
            console.log(resp);
        });

    };

    //保存修改后的数据
    $scope.Save = function () {
        $scope.isSubmit = true;
        if (!$scope.upForm.$valid)
            return;

        $scope.preProcess.processJson = JSON.stringify($scope.preProcess.Process);
        if ($scope.preProcess.id) {
            $http.post(GetUrl('api/PreProcess/Update'), $scope.preProcess).then(function (res) {
                $('#detailModal').modal('hide');
                $scope.search();
            }).catch(function (resp) {
                console.log(resp);
            });
        } else {
            $scope.preProcess.UnProcessId = $scope.detailUnProcess.id;
            $http.post(GetUrl('api/PreProcess/Add'), $scope.preProcess).then(function (res) {
                $('#detailModal').modal('hide');
                $scope.search();
            }).catch(function (resp) {
                console.log(resp);
            });
        }
    };
});