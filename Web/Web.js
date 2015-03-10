if (Meteor.isServer) {

    Movies = new Meteor.Collection("movies");

    // Seed the movie database with a few movies
    Meteor.startup(function () {
        if (Movies.find().count() == 0) {
            Movies.insert({title: "Star Wars", director: "Lucas", "rating": [{"stars": 1}, {"stars": 2}]});
            Movies.insert({title: "Memento", director: "Nolan"});
            Movies.insert({title: "King Kong", director: "Jackson"});
        }
    });

    Meteor.publish("allMovies", function () {
        console.log("allMovies");
        return Movies.find(); // everything
    });

    Meteor.methods({

        helloWorld: function () {
            console.log("helloWorld");
            Movies.insert({title: "Hello from iOS", director: "Nic and Phil"});
            return "HELLOOOO";
        }
    });
}