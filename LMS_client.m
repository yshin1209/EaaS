% LMS (Least Mean Squares) Adaptive Parameter Estimation web service client
% Yong-Jun Shin (2018)
% For more information, please visit https://github.com/yshin1209/EaaS

N = 100;                       % number of iteration
x = zeros(N,1);                % output signal x
u = 10*ones(N,1) + randn(N,1); % input signal u
parameter = 0.837;             % parameter value
response = zeros(N,3);         % web service response [estiated output, estimation error, estimated parameter]
newServiceId = '1';            % new Actor service ID
timeDelay = 1;                 % time dealy between the input and the output
timeDelay_value = num2str(timeDelay);  % convert timeDelay into string
stepSize = 0.001;              % LMS step size
stepSize_value = num2str(stepSize);    % convert stepSize into string
timeCom = zeros(N,1);          % time for the round-trip between the client and the cloud

% initialize LMS web servie with newServiceId and timeDelay_value
url = ['http://csmlab8.uconn.edu/api/lms/' newServiceId '/' timeDelay_value];
webread(url); 

for n = timeDelay+1:N
    % simulated x which takes input signal u as an input
    x(n) = parameter*u(n-timeDelay) + randn(1,1);
    
    % convert x and r into string
    x_value = num2str(x(n));
    u_value = num2str(u(n));

    % call the LMS web service which returns response [estiated output, estimation error, estimated parameter]
    url = ['http://csmlab7.uconn.edu/api/lms/' newServiceId '/' stepSize_value '/' u_value '/' x_value];
    tic
    response(n,:) = webread(url);
    timeCom(n) = toc; % time for the round-trip between the client and the cloud
end

estOutput = response(:,1);
estError = response(:,2);
estParameter = response(:,3);

subplot(2,1,1);
plot(1:N, x, 'b', 1:N, estOutput, 'g', 1:N, estError, 'r', 1:N, estParameter, '*');
legend('x (output)', 'estimated x', 'estimation error', 'estimated parameter');
xlabel('time (n)');
ylabel('magnitude (arbitrary unit)');
parameter_value = num2str(parameter);
txt1 = ['true parameter value = ' parameter_value] ;
text(30,3,txt1);
axis([0 N -5 15]);

subplot(2,1,2);
plot (1:N, timeCom, '*')
xlabel('time (n)');
ylabel('round-trip time (second)');
