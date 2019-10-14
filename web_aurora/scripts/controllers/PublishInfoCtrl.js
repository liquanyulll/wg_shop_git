app.controller('PublishInfoCtrl', ['$scope', '$http', "ModalService", function ($scope, $http, ModalService) {
    $scope.publishInfos = [];
    $scope.queryParams = {};

    $scope.paginationConf = {
        currentPage: 1,
        itemsPerPage: 15
    };
    $scope.paginationConf.perPageOptions = [15, 20, 50, 100];

    //加载国家下拉框
    $http.get(GetUrl('api/MBCommon/GetCountrys')).then(function (res) {
        $scope.countrys = res.data;
    }).catch(function (resp) {
        console.log(resp);
    });

    $scope.GetStateName = function (val) {
        switch (val) {
            case "Y":
                return "已发布";
            case "N":
                return "待发布";
            default:
                return "未知";
        }
    };

    //查询列表数据
    $scope.search = function () {
        $scope.queryParams.pageIndex = $scope.paginationConf.currentPage;
        $scope.queryParams.pageSize = $scope.paginationConf.itemsPerPage;

        $http.post(GetUrl('api/PublicInfo/Search'), $scope.queryParams).then(function (res) {
            console.log(res);
            $scope.publishInfos = res.data.data;
            $scope.paginationConf.totalItems = res.data.total;
        }).catch(function (resp) {
            console.log(resp);
        });
    };

    /* 监听数据类型切换及换页 */
    $scope.$watch('paginationConf.currentPage + paginationConf.itemsPerPage', function (v, a) {
        $scope.search();
    });

    //监听数据是否改变
    $scope.$watch('publishInfo', function (newValue, oldValue) {
        if ($scope.selectedX) {
            $scope.unChanged = angular.equals($scope.selectedX, newValue);
        }
    }, true);

    //审核通过
    $scope.Publish = function (x) {
        ModalService.Confirm("发布后不可撤销，是否确认发布到主库?", function () {
            $http.post(GetUrl('api/PreProcess/Audit?id=' + x.id)).then(function (res) {
                console.log(res);
            }).catch(function (resp) {
                console.log(resp);
            });
        });
    };

    /// 弹层相关
    $scope.publishInfo = {};

    //弹出详情框框
    $scope.Update = function (x) {
        $scope.isSubmit = false;
        $scope.selectedX = x;
        $scope.publishInfo = angular.copy($scope.selectedX);
        $('#detailModal').modal({
            keyboard: true,
            backdrop: true
        });
    };

    $scope.Add = function () {
        $scope.isSubmit = false;
        $scope.selectedX = {};
        $scope.publishInfo = {};
        $('#detailModal').modal({
            keyboard: true,
            backdrop: true
        });
    };

    $scope.Delete = function (x) {
        ModalService.Confirm("是否确认删除?", function () {
            $http.post(GetUrl('api/PublicInfo/Delete?id=' + x.id)).then(function (res) {
                $scope.search();
            }).catch(function (resp) {
                console.log(resp);
            });
        });
    };

    //保存修改后的数据
    $scope.Save = function () {
        $scope.isSubmit = true;
        if (!$scope.piForm.$valid)
            return;

        if ($scope.selectedX.id) {
            $http.post(GetUrl('api/PublicInfo/Update'), $scope.publishInfo).then(function (res) {
                $('#detailModal').modal('hide');
                $scope.search();
            }).catch(function (resp) {
                console.log(resp);
            });
        } else {
            console.log($scope.publishInfo);
            $http.post(GetUrl('api/PublicInfo/Add'), $scope.publishInfo).then(function (res) {
                $('#detailModal').modal('hide');
                $scope.search();
            }).catch(function (resp) {
                console.log(resp);
            });
        }

    };
}]);