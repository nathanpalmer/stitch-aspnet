Template = require("views/view")

class Test
    constructor: ->
        @obj = { name: "Nathan" }

    append: ->
        Template(@obj).appendTo('#content')

module.exports = Test