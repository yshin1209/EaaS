% Yong-Jun Shin (2018)
% BLAS (Basic Linear Algebra Subprograms) 1
% Vector-Vector Multiplication
% For more information, please see http://www.netlib.org/blas/

clc;
clear all;

id = 1;
N = 10;
vector1 = rand(N,1);
vector2 = rand(N,1);
url = 'http://csmlab8.uconn.edu/api/la/blas1';
data = struct('Id', id, 'Vector1', vector1, 'Vector2', vector2);
options = weboptions('MediaType','application/json');
response = webwrite (url, data, options)

%%
% BLAS (Basic Linear Algebra Subprograms) 2
% Matrix-Vector Multiplication
% For more information, please see http://www.netlib.org/blas/

clc;
clear all;

id = 1;
N = 10;
matrix = rand(N,N);
vector = rand(N,1);
url = 'http://csmlab8.uconn.edu/api/la/blas2';
data = struct('Id', id, 'Matrix', matrix, 'Vector', vector);
options = weboptions('MediaType','application/json');
response = webwrite (url, data, options)


%%
% BLAS (Basic Linear Algebra Subprograms) 3
% Matrix-Matrix Multiplication
% For more information, please see http://www.netlib.org/blas/

clc;
clear all;

id = 1;
N = 10
matrix1 = rand(N,N);
matrix2 = rand(N,N);
url = 'http://csmlab8.uconn.edu/api/la/blas3';
data = struct('Id', id, 'Matrix1', matrix1, 'Matrix2', matrix2);
options = weboptions('MediaType','application/json');
response = webwrite (url, data, options)
