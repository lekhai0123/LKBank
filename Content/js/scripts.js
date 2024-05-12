
var textWrapper = document.querySelector('.ml11 .letters');
textWrapper.innerHTML = textWrapper.textContent.replace(/([^\x00-\x80]|\w)/g, "<span class='letter'>$&</span>");

anime.timeline({loop: true})
  .add({
    targets: '.ml11 .line',
    scaleY: [0,1],
    opacity: [0.5,1],
    easing: "easeOutExpo",
    duration: 700
  })
  .add({
    targets: '.ml11 .line',
    translateX: [0, document.querySelector('.ml11 .letters').getBoundingClientRect().width + 10],
    easing: "easeOutExpo",
    duration: 700,
    delay: 100
  }).add({
    targets: '.ml11 .letter',
    opacity: [0,1],
    easing: "easeOutExpo",
    duration: 600,
    offset: '-=775',
    delay: (el, i) => 34 * (i+1)
  }).add({
    targets: '.ml11',
    opacity: 0,
    duration: 1000,
    easing: "easeOutExpo",
    delay: 1000
  });

var nav = document.querySelector('nav');
    
          window.addEventListener('scroll', function () {
          
            if (window.pageYOffset > 0) {
              nav.classList.add('bg-white', 'shadow');
              $(".nav-link").css("color", "black");
              $(".navbar-toggler").css("color", "black");
              $(".navbar-toggler").css("border-color", "black");
              $(".fas").css("color", "black");
              $(".navbar-brand").css("color", "black");
            } else if (window.pageYOffset < 10){
              nav.classList.remove('bg-white', 'shadow');
              $(".nav-link").css("color", "white");
              $(".navbar-toggler").css("color", "white");
              $(".navbar-toggler").css("border-color", "white");
              $(".fas").css("color", "white");
              $(".navbar-brand").css("color", "white");
            }
            
          });

window.addEventListener('resize', function(event){
            
  const mq = window.matchMedia( "(min-width: 770px)" );

  if (mq.matches) {
    nav.classList.remove('bg-white', 'shadow');
    $(".nav-link").css("color", "white");
    $(".navbar-toggler").css("color", "white");
    $(".navbar-toggler").css("border-color", "white");
    $(".fas").css("color", "white");
    $(".navbar-brand").css("color", "white");
  } else {
    nav.classList.add('bg-white', 'shadow');
    $(".nav-link").css("color", "black");
    $(".navbar-toggler").css("color", "black");
    $(".navbar-toggler").css("border-color", "black");
    $(".fas").css("color", "black");
    $(".navbar-brand").css("color", "black");
  }
});        
       
            

          
          
        