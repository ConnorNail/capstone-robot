Open Terminal

Start Rosbridge:
source /opt/ros/noetic/setup.bash
roslaunch rosbridge_server rosbridge_websocket.launch

Start Listener Topic:
source ~/ws_moveit/devel/setup.bash
rosrun capstone listener
rosrun moveit_tutorials listener.py *new*

Start MoveIt/RViz
roslaunch capstone_moveit_config demo.launch
rosrun moveit_tutorials move_group_python_interface_tutorial.py

If Updates Are Made:
cd ~/ws_moveit
catkin_make

New Steps:
roslaunch capstone_moveit_config demo_gazebo.launch
roslaunch capstone_moveit_config moveit_servo.launch

Final Steps:
roslaunch capstone_moveit_config demo_gazebo.launch
Wait!
roslaunch capstone_moveit_config moveit_servo.launch
Reset
Set to non-singular position
Plan and Execute
roslaunch rosbridge_server rosbridge_websocket.launch
rosrun moveit_tutorials servoListener.py
Start Capstone on Oculus headset
