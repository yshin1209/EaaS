N = 63;           % number of iteration
x =zeros(N,1);    % measured data x
u = zeros(N,1);   % control signal u
r =60*sin(-pi:0.1:pi)+200 % reference (target) values
newServiceId = '3'; % new Actor service ID
Kp =num2str(2);    % proportional parameter Kp
Ki = num2str(2);   % integral parameter Ki
Kd = num2str(0.8); % derivative parameter Kd
for n= 3:N

    if n> 50 d=60; % add disturbance (d) to x at time > 50
    else d=0;
    end
    
    x(n) =0.4*u(n-1)+0.6*x(n-1) + d; % simulated x which takes control signal u as an input
    x_value = num2str(x(n));
    r_value = num2str(r(n));
    
    if n==3 
        reset_value = 'true'; % reset the stored values (on the cloud) to zero in the beginning
    else
        reset_value = 'false';
    end
    
    url =['http://csmlab8.uconn.edu/api/pid/' newServiceId '/' reset_value '/' x_value '/' r_value '/' Kp '/' Ki '/' Kd];
    u(n) =webread(url); % call PID web service which returns control signal u(n)
    
end

figure
plot(1:N, r, 'r', 1:N, x, 'b', 1:N, u, 'g');
legend('r (reference)', 'x (output)', 'u (control)');
xlabel('time(n)');
ylabel('magnitude (arbitrary unit)');
axis([0 63 0 450]);
