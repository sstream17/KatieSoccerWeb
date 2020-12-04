mergeInto(LibraryManager.library, {
  Ready: function() {
    initializeGame();
  }, 
  UpdateScore: function(id, score) {
    document.getElementById(id).textContent = score;
  }
});