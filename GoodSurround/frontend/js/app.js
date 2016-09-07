angular.module('GoodSurround', ['ngRoute', 'ui.bootstrap']);

angular.module('GoodSurround').config(['$routeProvider', '$httpProvider', '$locationProvider',
function ($routeProvider, $httpProvider, $locationProvider) {
    $routeProvider.
    when('/login', {
        templateUrl: 'templates/login.html',
        controller: 'LoginController'
    }).
    when('/users', {
        templateUrl: 'templates/users.html',
        controller: 'UsersController'
    }).
    when('/schedule', {
        templateUrl: 'templates/schedule.html',
        controller: 'ScheduleController'
    }).
    otherwise({
        redirectTo: '/'
    });

    //$locationProvider.html5Mode({
    //    enabled: true,
    //    requireBase: false
    //});
}]);
