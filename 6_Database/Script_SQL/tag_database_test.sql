#--- TEST 1 DATA INSERT ---
INSERT INTO `user`(`nickname`, `email`, `password`) VALUES ('test_01', 'test_01@tag.com', 'i_am_not_encrypted_but_if_user_use_app_i_will_be_encrypted');
INSERT INTO `user`(`nickname`, `email`, `password`) VALUES ('test_02', 'test_02@tag.com', 'Password&1');

INSERT INTO `match`(`date_played`) VALUES (now());

INSERT INTO `plays`(`score`, `times_tagged_someone`, `times_tagged`, `user_id`, `match_id`) VALUES ('1200', '10', '3', '1', '1');
INSERT INTO `plays`(`score`, `times_tagged_someone`, `times_tagged`, `user_id`, `match_id`) VALUES ('350', '5', '9', '2', '1');

#--- TEST 1.1 DATA SELECT ---
SELECT * FROM `user`;
SELECT * FROM `match`;
SELECT * FROM `plays`;