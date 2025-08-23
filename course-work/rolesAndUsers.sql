CREATE ROLE c##app_user;
CREATE ROLE c##hotel_manager;
CREATE ROLE c##hotel_admin;

GRANT EXECUTE ON register_user TO PUBLIC;
GRANT EXECUTE ON get_available_rooms TO PUBLIC;
GRANT EXECUTE ON get_room_reviews TO PUBLIC;

GRANT EXECUTE ON book_room TO c##app_user;
GRANT EXECUTE ON cancel_booking TO c##app_user;
GRANT EXECUTE ON add_review TO c##app_user;
GRANT EXECUTE ON add_service TO c##app_user;
GRANT EXECUTE ON get_user_bookings TO c##app_user;
GRANT EXECUTE ON get_all_services TO c##app_user;

GRANT EXECUTE ON change_booking_status TO c##hotel_manager;
GRANT EXECUTE ON update_room_price TO c##hotel_manager;
GRANT EXECUTE ON remove_service_from_booking TO c##hotel_manager;
GRANT EXECUTE ON get_user_bookings TO c##hotel_manager;
GRANT EXECUTE ON get_all_services TO c##hotel_manager;
GRANT EXECUTE ON book_room TO c##hotel_manager;
GRANT EXECUTE ON cancel_booking TO c##hotel_manager;
GRANT EXECUTE ON add_service TO c##hotel_manager;

GRANT EXECUTE ON toggle_user_block TO c##hotel_admin;
GRANT EXECUTE ON assign_manager_role TO c##hotel_admin;
GRANT EXECUTE ON delete_user TO c##hotel_admin;
GRANT EXECUTE ON add_room TO c##hotel_admin;
GRANT EXECUTE ON update_room TO c##hotel_admin;
GRANT EXECUTE ON delete_room TO c##hotel_admin;
GRANT EXECUTE ON add_new_service TO c##hotel_admin;
GRANT EXECUTE ON update_service TO c##hotel_admin;
GRANT EXECUTE ON delete_service TO c##hotel_admin;
GRANT EXECUTE ON get_all_services TO c##hotel_admin;

CREATE USER c##client1 IDENTIFIED BY "UserPassword123";
CREATE USER c##manager1 IDENTIFIED BY "ManagerPass123";
CREATE USER c##admin1 IDENTIFIED BY "AdminPass123";

GRANT c##app_user TO c##client1;
GRANT c##hotel_manager TO c##manager1;
GRANT c##hotel_admin TO c##admin1;

GRANT CREATE SESSION TO c##app_user, c##hotel_manager, c##hotel_admin;

GRANT SELECT ON Rooms TO c##app_user, c##hotel_manager;
GRANT SELECT ON RoomTypes TO c##app_user, c##hotel_manager;
GRANT SELECT ON RoomServices TO c##app_user, c##hotel_manager;
GRANT SELECT, INSERT, UPDATE ON Bookings TO c##hotel_manager;
GRANT SELECT, INSERT ON Reviews TO c##app_user;
GRANT SELECT, INSERT ON BookingServices TO c##app_user, c##hotel_manager;

GRANT SELECT, INSERT, UPDATE, DELETE ON Users TO c##hotel_admin;
GRANT SELECT, INSERT, UPDATE, DELETE ON Rooms TO c##hotel_admin;
GRANT SELECT, INSERT, UPDATE, DELETE ON RoomTypes TO c##hotel_admin;
GRANT SELECT, INSERT, UPDATE, DELETE ON RoomServices TO c##hotel_admin;
GRANT SELECT, INSERT, UPDATE, DELETE ON Bookings TO c##hotel_admin;
GRANT SELECT, INSERT, UPDATE, DELETE ON BookingServices TO c##hotel_admin;
GRANT SELECT, INSERT, UPDATE, DELETE ON Reviews TO c##hotel_admin;

GRANT EXECUTE ON book_room TO c##client1;
GRANT EXECUTE ON cancel_booking TO c##client1;
GRANT EXECUTE ON add_review TO c##client1;
GRANT EXECUTE ON add_service TO c##client1;
GRANT EXECUTE ON get_user_bookings TO c##client1;

GRANT EXECUTE ON change_booking_status TO c##manager1;
GRANT EXECUTE ON update_room_price TO c##manager1;
GRANT EXECUTE ON remove_service_from_booking TO c##manager1;
GRANT EXECUTE ON get_user_bookings TO c##manager1;

GRANT EXECUTE ON toggle_user_block TO c##admin1;
GRANT EXECUTE ON assign_manager_role TO c##admin1;
GRANT EXECUTE ON delete_user TO c##admin1;
GRANT EXECUTE ON add_room TO c##admin1;
GRANT EXECUTE ON update_room TO c##admin1;
GRANT EXECUTE ON delete_room TO c##admin1;
GRANT EXECUTE ON add_new_service TO c##admin1;
GRANT EXECUTE ON update_service TO c##admin1;
GRANT EXECUTE ON delete_service TO c##admin1;

GRANT EXECUTE ON register_user TO c##client1, c##manager1, c##admin1;
GRANT EXECUTE ON get_available_rooms TO c##client1, c##manager1, c##admin1;
GRANT EXECUTE ON get_room_reviews TO c##client1, c##manager1, c##admin1;

GRANT CREATE SESSION TO c##client1, c##manager1, c##admin1;

GRANT SELECT ON Rooms TO c##client1;
GRANT SELECT ON RoomTypes TO c##client1;
GRANT SELECT ON RoomServices TO c##client1;
GRANT SELECT, INSERT ON Reviews TO c##client1;
GRANT SELECT, INSERT ON BookingServices TO c##client1;

GRANT SELECT ON Rooms TO c##manager1;
GRANT SELECT ON RoomTypes TO c##manager1;
GRANT SELECT ON RoomServices TO c##manager1;
GRANT SELECT, INSERT, UPDATE ON Bookings TO c##manager1;
GRANT SELECT, INSERT ON BookingServices TO c##manager1;

GRANT SELECT, INSERT, UPDATE, DELETE ON Users TO c##admin1;
GRANT SELECT, INSERT, UPDATE, DELETE ON Rooms TO c##admin1;
GRANT SELECT, INSERT, UPDATE, DELETE ON RoomTypes TO c##admin1;
GRANT SELECT, INSERT, UPDATE, DELETE ON RoomServices TO c##admin1;
GRANT SELECT, INSERT, UPDATE, DELETE ON Bookings TO c##admin1;
GRANT SELECT, INSERT, UPDATE, DELETE ON BookingServices TO c##admin1;
GRANT SELECT, INSERT, UPDATE, DELETE ON Reviews TO c##admin1;
