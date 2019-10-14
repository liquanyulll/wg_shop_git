app.controller('ProductListCtrl', function ($scope, $http, $rootScope) {
    $scope.paginationConf = {
        currentPage: 1,
        itemsPerPage: 20
    };
    $scope.paginationConf.perPageOptions = [20];
    $scope.paginationConf.totalItems = 30;

    //$rootScope.www = "7777777";

});
