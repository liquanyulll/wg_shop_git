app.config(['$routeProvider', "$locationProvider", "$httpProvider", function ($routeProvider, $locationProvider, $httpProvider) {
    //这是因为Angular 1.6 版本更新后 对路由做的处理，这样才可以和以前版本一样正常使用
    $locationProvider.hashPrefix('');
    //注册拦截器
    $httpProvider.interceptors.push("httpInterceptor");

    $routeProvider
        .when('/productList', {
            /**/
            templateUrl: 'views/product-list.html',
            /**/
            controller: "ProductListCtrl"
        }).when('/product/:id/:invcode?', {
            /**/
            templateUrl: 'views/product.html',
            /**/
            controller: "ProductCtrl"
        }).when('/my', {
            /**/
            templateUrl: 'views/my.html',
            /**/
            controller: "MyCtrl"
        })//).when('/webSite', {
        //    /**/
        //    templateUrl: 'views/web-site.html',
        //    /**/
        //    controller: "WebSiteCtrl"
        //}).when('/preProcess', {
        //    /**/
        //    templateUrl: 'views/pre-process.html',
        //    /**/
        //    controller: "PreProcessCtrl"
        //}).when('/publishInfo', {
        //    /**/
        //    templateUrl: 'views/publish-info.html',
        //    /**/
        //    controller: "PublishInfoCtrl"
        //}).when('/login', {
        //    /**/
        //    templateUrl: 'views/login.html',
        //    /**/
        //    controller: "LoginCtrl"
        //})
        .otherwise({
            redirectTo: 'productList'
        });

}]);