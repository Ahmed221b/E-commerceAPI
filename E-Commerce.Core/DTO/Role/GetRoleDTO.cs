namespace E_Commerce.Core.DTO.Role
{
    public class GetRoleDTO
    {
        public string RoleName { get; set; }
        public List<UsersInRoleDTO> usersInRoleDTOs { get; set; }
    }
}
