% PID (Proportional-Integral_Derivative) web service client
% Yong-Jun Shin (2018)
% For more information, please visit https://github.com/yshin1209/EaaS

N = 63;                     % number of iteration
x = zeros(N,1);             % output (measured) signal x
u = zeros(N,1);             % control signal u
r = 60*sin(-pi:0.1:pi)+200; % reference (target) signal values
newServiceId = '1';         % new Actor service ID
Kp = num2str(2);            % the proportional parameter Kp
Ki = num2str(2);            % the integral parameter Ki
Kd = num2str(0.8);          % the derivative parameter Kd
timeCom = zeros(N,1);       % time for the round-trip between the client and the cloud

for n= 2:N
    % add disturbance (d) to x at time > 50
    if n > 50 
        d=60; 
    else
        d=0;
    end
    
    % simulate x which takes control signal u as an input
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
    url = ['http://csmlab7.uconn.edu/api/pid/' newServiceId '/' reset_value '/' x_value '/' r_value '/' Kp '/' Ki '/' Kd];
    tic;
    u(n) = webread(url); 
    timeCom(n) = toc; % time for the round-trip between the client and the cloud
end
subplot(2,1,1);
plot(1:N, r, 'r', 1:N, x, 'b', 1:N, u, 'g');
legend('r (reference)', 'x (output)', 'u (control)');
xlabel('time(n)');
ylabel('magnitude (arbitrary unit)');
axis([0 N 0 450]);

subplot(2,1,2);
plot (1:N, timeCom, '*')
xlabel('time (n)');
ylabel('round-trip time (second)');
