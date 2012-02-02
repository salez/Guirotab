http://www.trendskitchens.co.nz/jquery/contextmenu/


// exemplo 1 - html

<div class="contextMenu" id="myMenu1">

      <ul>

        <li id="open"><img src="folder.png" /> Open</li>

        <li id="email"><img src="email.png" /> Email</li>

        <li id="save"><img src="disk.png" /> Save</li>

        <li id="close"><img src="cross.png" /> Close</li>

      </ul>

    </div>

// exemplo 1 - script

$('span.demo1').contextMenu('myMenu1', {

  bindings: {

    'open': function(t) {

      alert('Trigger was '+t.id+'\nAction was Open');

    },

    'email': function(t) {

      alert('Trigger was '+t.id+'\nAction was Email');

    },

    'save': function(t) {

      alert('Trigger was '+t.id+'\nAction was Save');

    },

    'delete': function(t) {

      alert('Trigger was '+t.id+'\nAction was Delete');

    }
  }
});

//exemplo 2 - html

<div class="contextMenu" id="myMenu2">

    <ul>

      <li id="item_1">Item 1</li>

      <li id="item_2">Item 2</li>

      <li id="item_3">Item 3</li>

      <li id="item_4">Item 4</li>

      <!-- etc... -->

    </ul>

  </div>

////exemplo 2 - script

$('span.demo3').contextMenu('myMenu3', {

    onContextMenu: function(e) {

        if ($(e.target).attr('id') == 'dontShow') return false;

        else return true;

    },

    onShowMenu: function(e, menu) {
    
        if ($(e.target).attr('id') == 'showOne') {

            $('#item_2, #item_3', menu).remove();

        }

        return menu;
    }
});
