angular.module('GoodSurround').service('Users', ['$http', function ($http) {
    return {
        searchUser: function (searchString) {
            return new Promise(function (resolve) {
                $http.get('/api/v1/users/getUsers', {
                    params: {
                        searchString: searchString,
                        skip: 0,
                        take: 50,
                    }
                }).then(function (response) {
                    return resolve(response);
                });
            });
        }
    }
}]);
