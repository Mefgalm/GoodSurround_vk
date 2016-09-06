angular.module('GoodSurround').service('Users', [function () {
    var savedUsers = [{
        avatar: 'http://a3.mzstatic.com/us/r30/Purple/v4/e0/9d/64/e09d64ec-b4f1-5e55-3599-308e29d5a94d/icon100x100.png',
        name: 'Dmitry Outstanding',
        albumsCount: 34,
        songsCount: 7989
    }, {
        avatar: 'http://chandra.harvard.edu/photo/2016/gj3253/dwarfs_gj3253_xray_thm100.jpg',
        name: 'Dmitry Glorious',
        albumsCount: 4,
        songsCount: 789
    }, {
        avatar: 'http://a4.mzstatic.com/us/r30/Purple5/v4/12/a1/ef/12a1ef25-7ce6-d918-8bb9-07e5aa92fc65/icon100x100.jpeg',
        name: 'Dmitry Fabulous',
        albumsCount: 3,
        songsCount: 105
    }, {
        avatar: 'https://yt3.ggpht.com/-5mSnQcMR1E8/AAAAAAAAAAI/AAAAAAAAAAA/cW36ovOzVZw/s100-c-k-no-mo-rj-c0xffffff/photo.jpg',
        name: 'Here comes some incredibly long name which purpose is only to test it\'s displaying',
        albumsCount: 7,
        songsCount: 783
    }];

    var globalUsers = (new Array(100)).fill(0).map(function () {
        return {
            avatar: chance.avatar({
                protocol: 'https',
                fileExtension: 'jpg'
            }),
            name: chance.name(),
            albumsCount: Math.floor(Math.random() * 100),
            songsCount: Math.floor(Math.random() * 1000)
        };
    });

    function getSavedUsers () {
        return new Promise(function (resolve) {
            // reject is unnecessary: if error, return []
            // hereinafter, will be replaced with requests to server
            return resolve(savedUsers);
        });

    }

    function isUser (something) {
        return true;
    }

    function addUsersToSaved (param) {
        return new Promise(function (resolve) {
            var newUsers = (Array.isArray(param))
                ? param
                : [ param ];
            newUsers.forEach(function (user) {
                if ((isUser(user)) && (!_.contains(savedUsers, user))) {
                    savedUsers.push(user);
                }
            });
            return resolve();
        });
    }

    function find (searchQuery) {
        return new Promise(function (resolve) {
            var searchResults = globalUsers.filter(function (user) {
                return user.name.toLowerCase().indexOf(searchQuery) !== -1;
            });
            return resolve(searchResults);
        });
    }

    return {
        saved: {
            getAll: getSavedUsers,
            add: addUsersToSaved
        },
        find: find
    };
}]);
