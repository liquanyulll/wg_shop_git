var services = angular.module('services', []);

services.service('WebSiteService', ['$http', function ($http) {
    this.aa = 1;
    //var doRequest = function (url, callBack) {
    //    $http.post(url, null).then(function (res) {
    //        if (callBack) {
    //            callBack(res, url);
    //        }
    //    }).catch(function (res) {
    //        res.data.Message += "地址:" + url + " --更新失败请手动更新！！";
    //        HttpCatchService.CatchError(res);
    //    });
    //};
}]);

services.service('ModalService', ["$modal", function ($modal) {

    //Msg 提示信息
    //cb 回调函数
    //header 头部信息，可不填
    this.Confirm = function (msg, cb, header) {
        header = header || "提示";
        var model = {
            msg: msg,
            cb: cb,
            header: header
        };

        $modal.open({
            templateUrl: '/views/template/confirm.html',
            backdrop: 'static',
            controller: 'ConfirmCtrl',
            size: 'mg',//??
            resolve: {
                model: function () { return angular.copy(model); }
            }
        });
    };

    this.Message = function (msg, header) {
        header = header || "提示";
        var model = {
            msg: msg,
            header: header
        };

        $modal.open({
            templateUrl: '/views/template/message.html',
            backdrop: 'static',
            controller: 'MessageCtrl',
            size: 'mg',//??
            resolve: {
                model: function () { return angular.copy(model); }
            }
        });
    };
}]);

services.service("HttpCatchService", ['ModalService', function (modalService) {
    this.CatchError = function (resp) {
        modalService.Message(resp.data, resp.status);
        return;
    };
}]);

services.controller("ConfirmCtrl", ['$scope', '$modalInstance', 'model', function ($scope, $modalInstance, model) {
    $scope.model = model;
    $scope.ok = function () {
        model.cb();
        $modalInstance.close(true);
    };
    $scope.cancel = function () {
        $modalInstance.close(true);
    };
}]);

services.controller("MessageCtrl", ['$scope', '$modalInstance', 'model', function ($scope, $modalInstance, model) {
    $scope.model = model;
    $scope.ok = function () {
        $modalInstance.close(true);
    };
}]);