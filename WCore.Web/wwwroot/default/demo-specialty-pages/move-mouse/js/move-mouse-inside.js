 (function($){
  "use strict"; 
     $(document).ready(function() {
        var W = window.innerWidth,
            H = window.innerHeight,
            p = 0,
            p2 = 0,
            particleCount = 200,
            particles = [],
            minDist = 120,
            dist,
            bgColor =  hexToRgba($('#canvas').attr("data-color")), // marsala of course
            dotColor = "#000",
            xSpeed = 5,
            ySpeed = 5,
            dotSize = 13; 

        // RequestAnimFrame for smooth animation
        window.requestAnimFrame = (function() {
            return window.requestAnimationFrame ||
                window.webkitRequestAnimationFrame ||
                window.mozRequestAnimationFrame ||
                window.oRequestAnimationFrame ||
                window.msRequestAnimationFrame ||
                function(callback) {
                    window.setTimeout(callback, 1000 / 60);
                };
        })();

         function hexToRgba(hex) {
                var c;
                if(/^#([A-Fa-f0-9]{3}){1,2}$/.test(hex)){
                    c= hex.substring(1).split('');
                    if(c.length== 3){
                        c= [c[0], c[0], c[1], c[1], c[2], c[2]];
                    }
                    c= '0x'+c.join('');
                    return 'rgba('+[(c>>16)&255, (c>>8)&255, c&255].join(',')+',1)';
                }
                throw new Error('Bad Hex');
            }

        // canvas
        var canvas = document.getElementById('canvas');
        // context
        var ctx = canvas.getContext('2d');

        // width + height
        canvas.width = W;
        canvas.height = H;

        // paint canvas
        function paintCanvas() {
            ctx.fillStyle = bgColor;
            ctx.fillRect(0, 0, W, H);
        }

        // particle stuff
        function Particle() {
            // position
            this.x = Math.random() * W;
            this.y = Math.random() * H;
            // speed
            this.vx = -1 + Math.random() * (Math.random() * xSpeed);
            this.vy = -1 + Math.random() * (Math.random() * ySpeed);
            // size
            this.radius = Math.random() * (Math.random() * dotSize);

            // draw them
            this.draw = function() {
                ctx.fillStyle = "rgba(0,0,0,0.2)";
                ctx.beginPath();
                ctx.arc(this.x, this.y, this.radius, 0, Math.PI * 2, false);

                // Fill the arc we just made
                ctx.fill();
            }
        }

        // to array
        for (var i = 0; i < particleCount; i++) {
            particles.push(new Particle());
        }

        // draw the things
        function draw() {
            paintCanvas();

            // draw particles
            for (var i = 0; i < particles.length; i++) {
                p = particles[i];
                p.draw();
            }

            // update
            update();
        }

        // make 'em move
        function update() {
            for (var i = 0; i < particles.length; i++) {
                p = particles[i];

                // change velocities
                p.x += p.vx;
                p.y += p.vy;

                // change position if leaves canvas
                if (p.x + p.radius > W)
                    p.x = p.radius;

                else if (p.x - p.radius < 0) {
                    p.x = W - p.radius;
                }

                if (p.y + p.radius > H)
                    p.y = p.radius;

                else if (p.y - p.radius < 0) {
                    p.y = H - p.radius;
                }

                // make them attract
                for (var j = i + 1; j < particles.length; j++) {
                    p2 = particles[j];
                    distance(p, p2);
                }
            }
        }
        var mouse = {
            x: 0,
            y: 0
        };
        document.addEventListener('mousemove', function(e) {
            mouse.x = e.clientX || e.pageX;
            mouse.y = e.clientY || e.pageY;
            //console.log(mouse.x, mouse.y);
        }, false);
      
        
      

        // distance between dots
        function distance(p1, p2) {
            var dist,
                dx = p1.x - mouse.x;
            var dy = p1.y - mouse.y;
            dist = Math.sqrt((dx * dx) + (dy * dy));

            // draw line if distance is smaller than minimum distance
            if (dist <= minDist) {
                // draw line
                ctx.beginPath();
                ctx.strokeStyle = "rgba(30,33,41," + (1.4 - (dist / minDist) / Math.random()) + ")";
                ctx.lineWidth = 1;
                ctx.moveTo(mouse.x, mouse.y);
                ctx.lineTo(p1.x, p1.y);
                ctx.stroke();
                ctx.closePath();

                // acceleration depending on distance
                var ax = dy / 50000,
                    ay = dy / 50000;

                // apply acceleration
                p1.vx -= ax;
                p1.vy -= ay;
            }

        }

        // start main animation loop
        function animloop() {
            draw();
            requestAnimFrame(animloop);
        }
        animloop();

    });
 })(jQuery);

 