N = 63;                     % number of iteration
x = zeros(N,1);             % output (measured) signal x
u = zeros(N,1);             % control signal u
r = 60*sin(-pi:0.1:pi)+200; % reference (target) values
newServiceId = '1';         % new Actor service ID
Kp = num2str(2);            % the proportional parameter Kp
Ki = num2str(2);            % the integral parameter Ki
Kd = num2str(0.8);          % the derivative parameter Kd

for n= 3:N
    % add disturbance (d) to x at time > 50
    if n> 50 d=60; 
    else d=0;
    end
    
    % simulated x which takes control signal u as an input
    x(n) =0.4*u(n-1)+0.6*x(n-1) + d; 
    
    % convert x and r into string
    x_value = num2str(x(n));
    r_value = num2str(r(n));
    
    % reset the stored values (on the cloud) to zero in the beginning
    if n==3 
        reset_value = 'true'; 
    else
        reset_value = 'false';
    end
    
    % call the PID web service which returns control signal u(n)
    url =['http://csmlab8.uconn.edu/api/pid/' newServiceId '/' reset_value '/' x_value '/' r_value '/' Kp '/' Ki '/' Kd];
    u(n) =webread(url);   
end

figure
plot(1:N, r, 'r', 1:N, x, 'b', 1:N, u, 'g');
legend('r (reference)', 'x (output)', 'u (control)');
xlabel('time(n)');
ylabel('magnitude (arbitrary unit)');
axis([0 63 0 450]);
