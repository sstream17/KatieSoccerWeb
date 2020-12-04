mergeInto(LibraryManager.library, {
  Ready: function() {
    initializeGame();
  }, 
  UpdateScore: function(id, score) {
    document.getElementById(Pointer_stringify(id)).textContent = score;
  }
});