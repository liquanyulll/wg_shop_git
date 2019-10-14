app.controller('WebSiteCtrl', function ($scope, $http) {
    $scope.webSites = [];
    $scope.Enable = "Y";
    $scope.WebSiteName = "";

    $scope.search = function () {
        $http.get(GetUrl('api/WebSite/Search?WebSiteName=' + $scope.WebSiteName + "&Enable=" + $scope.Enable)).then(function (res) {
            $scope.webSites = res.data;
        }).catch(function (resp) {
            console.log(resp);
        });
    };

    $scope.search();

    //监听数据是否改变
    $scope.$watch('webSite', function (newValue, oldValue) {
        if ($scope.selectedX) {
            $scope.unChanged = angular.equals($scope.selectedX, newValue);
        }
    }, true);

    // 弹出框
    $scope.Edit = function (x) {
        $scope.isSubmit = false;

        if (x) {
            $scope.selectedX = x;
            $scope.webSite = angular.copy(x);
        } else {
            $scope.webSite = {
                autoPublish: "N",
                needTranslate: "N",
                enable: "Y"
            };
        }

        $('#detailModal').modal({
            keyboard: true,
            backdrop: true
        });
    };


    $scope.Save = function () {
        $scope.isSubmit = true;
        if (!$scope.wsForm.$valid)
            return;

        if ($scope.webSite.id) {
            $http.post(GetUrl('api/WebSite/Update'), $scope.webSite).then(function (res) {
                //$('#detailModal').modal('hide');
                //$scope.search();
            }).catch(function (resp) {
                console.log(resp);
            });
        } else {
            $http.post(GetUrl('api/WebSite/Add'), $scope.webSite).then(function (res) {
                $('#detailModal').modal('hide');
                $scope.search();
            }).catch(function (resp) {
                console.log(resp);
            });
        }
    };
});