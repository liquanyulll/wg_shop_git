app.controller('ProductCtrl', function ($scope, $http, $rootScope, $routeParams, $location) {
    var pid = $routeParams.id;
    var invCode = $routeParams.invcode;
    if (invCode) {

    }
    if (!pid || isNaN(Number(pid))) {
        $location.path("/productList")
    }

});
