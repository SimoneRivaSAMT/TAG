<?php
class Application{
    private $url_controller = null;
    private $url_action = null;
    private $url_parameter_1 = null;
    private $url_parameter_2 = null;
    private $url_parameter_3 = null;

    public function __construct(){
        $this->splitUrl();
        if(file_exists("./application/controller/" . $this->url_controller . ".php")){
            require "./application/controller/" . $this->url_controller . ".php";
            $this->url_controller = new $this->url_controller();
            if(method_exists($this->url_controller, $this->url_action)){
                if(isset($this->url_parameter_3)){
                    $this->url_controller->{$this->url_action}($this->url_parameter_1, $this->url_parameter_2, $this->url_parameter_3);
                }elseif(isset($this->url_parameter_2)){
                    $this->url_controller->{$this->url_action}($this->url_parameter_1, $this->url_parameter_2);
                }elseif(isset($this->url_parameter_1)){
                    $this->url_controller->{$this->url_action}($this->url_parameter_1);
                }else{
                    $this->url_controller->{$this->url_action}();
                }
            }else{
                $this->url_controller->index();
            }
        }else{
            require "./application/controller/defaultController.php";
            $home = new defaultController();
            $home->index();
        }
    }

    private function splitUrl(){
        if(isset($_GET['url'])){
            $url = rtrim($_GET['url'], '/');
            $url = filter_var($url, FILTER_SANITIZE_URL);
            $url = explode('/', $url);

            $this->url_controller = ($url[0] ?? null);
            $this->url_action = ($url[1] ?? null);
            $this->url_parameter_1 = ($url[2] ?? null);
            $this->url_parameter_2 = ($url[3] ?? null);
            $this->url_parameter_3 = ($url[4] ?? null);
        }
    }
}