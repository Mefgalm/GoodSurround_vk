angular.module('GoodSurround', ['ngRoute']);

angular.module('GoodSurround').config(['$routeProvider', '$httpProvider', '$locationProvider',
function ($routeProvider, $httpProvider, $locationProvider) {
    $routeProvider.
    when('/login', {
        templateUrl: 'templates/login.html',
        controller: 'LoginController'
    }).
    when('/find', {
        templateUrl: 'templates/find.html',
        controller: 'FindController'
    }).
    otherwise({
        redirectTo: '/'
    });

    //$locationProvider.html5Mode({
    //    enabled: true,
    //    requireBase: false
    //});
}]);
