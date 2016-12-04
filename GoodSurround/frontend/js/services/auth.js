angular.module('GoodSurround').service('Auth', ['$http', function ($http) {
    return {
        registerUser: function (code) {
            return new Promise(function (resolve) {
                $http.post('/api/v1/auth/register', {
                    data: {
                        code: code
                    }
                }).then(function (response) {
                    return resolve(response);
                });
            });
        }
    }
}]);
